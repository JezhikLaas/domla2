import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { User } from '../shared/user';
import { ErrorMessages } from './login-user-error-messages';

declare var $: any;

@Component({
  selector: 'da-login-user',
  templateUrl: './login-user.component.html',
  styles: []
})

export class LoginUserComponent implements OnInit, AfterViewInit {
  loginForm: FormGroup;
  errors: { [key: string]: string};
  formValidation: boolean;

  constructor(private fb: FormBuilder) {
  }

  ngOnInit() {
    this.errors = {'hasErrors': 'false'};
    this.formValidation = false;

    this.loginForm = this.fb.group({
      login: this.fb.control(
        null,
        [
          Validators.required
        ]
      ),
      password: this.fb.control(
        null,
        [
          Validators.required
        ]
      )
    });

    this.loginForm.statusChanges.subscribe(
      () => this.updateErrorMessages()
    );
  }

  ngAfterViewInit() {
    $('#login').focus();
  }

  login() {
    this.formValidation = true;
    this.updateErrorMessages();
    if (this.errors['hasErrors'] === 'true') {
      return;
    }
  }

  updateErrorMessages() {
    this.errors = {'hasErrors': 'false'};
    for (const message of ErrorMessages) {
      const control = this.loginForm.get(message.forControl);
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
