import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ErrorMessages } from './register-user-error-messages';
import { UserRegistration } from '../shared/user-registration';
import { RegisterUserService } from '../shared/register-user.service';
import { RegisterSuccessComponent } from '../register-success/register-success.component';
import { ErrorDialogComponent } from '../shared/error-dialog/error-dialog.component';
import { LoaderComponent } from '../shared/loader/loader.component';

declare var $: any;

@Component({
  selector: 'um-register-user',
  templateUrl: './register-user.component.html',
  styles: [],
  providers: [
    RegisterSuccessComponent
  ]
})

export class RegisterUserComponent implements OnInit {
  @ViewChild(LoaderComponent) loader: LoaderComponent;
  registerForm: FormGroup;
  errors: { [key: string]: string};
  formValidation: Boolean;

  constructor(
    private fb: FormBuilder,
    private rus: RegisterUserService,
    private msg: RegisterSuccessComponent,
    private errorDialog: ErrorDialogComponent) {
  }

  ngOnInit() {
    $('.dropdown').dropdown();
    this.loader.useDimmer = false;

    this.errors = {'hasErrors': 'false'};
    this.formValidation = false;

    this.registerForm = this.fb.group({
      salutation: this.fb.control(
        null,
        [
          Validators.required
        ]
      ),
      title: '',
      firstname: '',
      lastname: this.fb.control(
        null,
        [
          Validators.required,
          Validators.minLength(3)
        ]
      ),
      username: this.fb.control(
        null,
        [
          Validators.required,
          Validators.minLength(8),
          this.usernameIsValid
        ]
      ),
      email: this.fb.control(
        null,
        [
          Validators.required,
          Validators.email
        ]
      )
    });

    this.registerForm.statusChanges.subscribe(
      () => this.updateErrorMessages()
    );
  }

  emailChanged(value: string) {
    if (this.registerForm.get('username').pristine) {
      const newValue = value ? value.toLowerCase() : '';
      this.registerForm.get('username').setValue(newValue);
    }
  }

  usernameIsValid(control: FormControl) {
    console.log('checking ...');
    if (Validators.pattern('^[a-z_]+[0-9a-z_\\\\-]*$')(control) === null) {
      return null;
    }
    if (Validators.email(control) === null) {
      return null;
    }

    return { usernameIsValid: 'Ungültige Eingabe' };
  }

  register() {
    this.formValidation = true;
    this.updateErrorMessages();
    if (this.errors['hasErrors'] === 'true') {
      return;
    }

    const registration = new UserRegistration(
      this.registerForm.get('salutation').value,
      this.registerForm.get('title').value,
      this.registerForm.get('firstname').value,
      this.registerForm.get('lastname').value,
      this.registerForm.get('username').value,
      this.registerForm.get('email').value
    );

    this.loader.show('Sende Registrierung...');
    this.rus.registerUser(
      registration,
      () => {
          this.loader.hide();
          this.msg.show();
        },
      (code: number, message: string) => {
        this.loader.hide();
        switch (code) {
          case 409:
            this.errors['username'] = `Dieser Benutzername ist bereits vergeben. Beachten Sie, dass nicht zwischen
 Groß- und Kleinschreibung unterschieden wird.`;
            break;
          case 301:
            this.errors['email'] = `Überprüfen Sie, ob Sie bereits eine Bestätigungsmail erhalten haben.
 Möglicherweise benötigen wir auch ein paar Tage zur Bearbeitung ihrer Anfrage.`;
            break;
          default:
            this.errorDialog.show('Fehler', message);
          }
      });
  }

  updateErrorMessages() {
    this.errors = {'hasErrors': 'false'};
    for (const message of ErrorMessages) {
      const control = this.registerForm.get(message.forControl);
      if (control && (this.formValidation || control.dirty) && control.invalid && control.errors[message.forValidator]) {
        if (!this.errors[message.forControl]) {
          this.errors[message.forControl] = message.message;
          this.errors['hasErrors'] = 'true';
        }
      }
    }
    this.formValidation = false;
  }
}
