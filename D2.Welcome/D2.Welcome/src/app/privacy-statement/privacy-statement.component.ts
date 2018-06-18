import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ErrorMessages } from './privacy-statement-errors';
import { FinishRegistration } from '../shared/finish-registration';
import { StorageService } from '../shared/storage.service';
import { FinishRegistrationService } from '../shared/finish-registration.service';
import { ErrorDialogComponent } from '../shared/error-dialog/error-dialog.component';
import { InfoDialogComponent } from '../shared/info-dialog/info-dialog.component';
import { LoaderComponent } from '../shared/loader/loader.component';

function checkIfPasswordsAreMatching(group: FormGroup) {
  // safety check
  if (!group.get('passwordOne').value || !group.get('passwordTwo').value) { return null; }

  if (group.get('passwordOne').value !== group.get('passwordTwo').value) {
    group.get('passwordTwo').setErrors({ matching: true });
    return { error: 'Passwörter stimmen nicht überein.' };
  }
  return null;
}

async function delay(ms: number) {
  return new Promise( resolve => setTimeout(resolve, ms) );
}

@Component({
  selector: 'app-privacy-statement',
  templateUrl: './privacy-statement.component.html',
  styleUrls: ['./privacy-statement.component.css']
})
export class PrivacyStatementComponent implements OnInit {

  @ViewChild(LoaderComponent) loader: LoaderComponent;
  public statementAccepted: boolean;
  public stepOneActive: boolean;
  public stepTwoActive: boolean;
  public errors: { [key: string]: string};
  public passwordGroup: FormGroup;
  public passwordOne: string;
  public passwordTwo: string;

  constructor(
    private storage: StorageService,
    private finishRegistration: FinishRegistrationService,
    private errorDialog: ErrorDialogComponent,
    private infoDialog: InfoDialogComponent
  ) { }

  ngOnInit() {
    this.loader.useDimmer = false;
    this.stepOneActive = true;
    this.errors = { 'hasErrors': 'false', 'passwordOne': null, 'passwordTwo': null };
    this.passwordGroup = new FormGroup({
      passwordOne: new FormControl(
        null,
        [
          Validators.required,
          Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[$@$!%*?&])[A-Za-z\\d$@$!%*?&]{8,}')
        ]
      ),
      passwordTwo: new FormControl(
        null,
        Validators.required,
      )
    },
      checkIfPasswordsAreMatching
    );

    this.passwordGroup.statusChanges.subscribe(
      () => this.updateErrorMessages()
    );

    this.updateErrorMessages();
  }

  acceptChange(value: boolean): void {
    this.statementAccepted = value;
  }

  state(): string {
    return this.statementAccepted ? '' : 'disabled';
  }

  maySubmit(): string {
    return this.errors['hasErrors'] === 'false' ? '' : 'disabled';
  }

  commitAcceptance(): void {
    this.stepOneActive = false;
    this.stepTwoActive = true;
  }

  sendRegistration(): void {
    this.loader.show('Schließe Registrierung ab...');
    const info = new FinishRegistration(
      this.storage.get('id'),
      this.passwordGroup.get('passwordOne').value as string
    );
    this.finishRegistration.finishRegistration(info).subscribe(
      result => {
        this.loader.hide();
        this.infoDialog.show(
          'Erfolg',
          'Die Registrierung wurde erfolgreich abgeschlossen. Sie werden gleich zur Anmeldung weitergeleitet.'
        );
        delay(2000).then(() => window.location.href = result.goto);
      },
      error => {
        this.loader.hide();
        this.errorDialog.show('Fehler', error.message);
      },
      () => this.loader.hide()
    );
  }

  updateErrorMessages() {
    this.errors = {'hasErrors': 'false'};
    for (const message of ErrorMessages) {
      const control = this.passwordGroup.get(message.forControl);
      if (control && (this.passwordGroup || control.dirty) && control.invalid && control.errors[message.forValidator]) {
        if (!this.errors[message.forControl]) {
          this.errors[message.forControl] = message.message;
          this.errors['hasErrors'] = 'true';
        }
      }
    }
  }
}
