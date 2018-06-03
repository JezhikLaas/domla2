import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ErrorMessages } from './privacy-statement-errors';

function checkIfPasswordsAreMatching(group: FormGroup) {
  // safety check
  if (!group.get('passwordOne').value || !group.get('passwordTwo').value) { return null; }

  if (group.get('passwordOne').value !== group.get('passwordTwo').value) {
    group.get('passwordTwo').setErrors({ matching: true });
    return { error: 'Passwörter stimmen nicht überein.' };
  }
  return null;
}

@Component({
  selector: 'app-privacy-statement',
  templateUrl: './privacy-statement.component.html',
  styleUrls: ['./privacy-statement.component.css']
})
export class PrivacyStatementComponent implements OnInit {

  public statementAccepted: boolean;
  public stepOneActive: boolean;
  public stepTwoActive: boolean;
  public errors: { [key: string]: string};
  public passwordGroup: FormGroup;
  public passwordOne: string;
  public passwordTwo: string;

  constructor() { }

  ngOnInit() {
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
