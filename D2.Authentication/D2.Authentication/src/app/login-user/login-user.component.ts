import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { AccountServiceService } from '../shared/account-service.service';
import { ErrorDialogComponent } from '../shared/error-dialog/error-dialog.component';
import { UserLogin } from '../shared/user-login';
import { ErrorMessages } from './login-user-error-messages';
import 'rxjs/add/operator/map';

declare var $: any;

@Component({
  selector: 'da-login-user',
  templateUrl: './login-user.component.html',
  styles: []
})

export class LoginUserComponent implements OnInit, AfterViewInit {
  returnUrl: string;
  loginForm: FormGroup;
  errors: { [key: string]: string };
  formValidation: boolean;
  errorDialog: ErrorDialogComponent;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private service: AccountServiceService,
  ) { }

  ngOnInit() {
    this.errorDialog = new ErrorDialogComponent();
    this.route
      .queryParamMap
      .map(params => params.get('returnUrl'))
      .subscribe(value => this.returnUrl = value);

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

    const user = new UserLogin(
      this.loginForm.get('login').value,
      this.loginForm.get('password').value,
      this.returnUrl
    );

    this.service.login(user, () => {}, (message) => {
      this.errorDialog.show('Fehler', message);
    });
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
