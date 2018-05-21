import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import base64url from 'base64url';
import { AccountServiceService } from '../shared/account-service.service';
import { ErrorDialogComponent } from '../shared/error-dialog/error-dialog.component';
import { LoaderComponent } from '../shared/loader/loader.component';
import { UserLogin } from '../shared/user-login';
import { ErrorMessages } from './login-user-error-messages';

declare var $: any;

@Component({
  selector: 'da-login-user',
  templateUrl: './login-user.component.html',
  styles: []
})

export class LoginUserComponent implements OnInit, AfterViewInit {

  @ViewChild(LoaderComponent) loader: LoaderComponent;
  private returnUrl: string;
  private token: string;
  loginForm: FormGroup;
  errors: { [key: string]: string };
  private formValidation: boolean;
  private loginInProcess: boolean;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private service: AccountServiceService,
    private errorDialog: ErrorDialogComponent
  ) { }

  ngOnInit() {
    this.route
      .queryParams
      .subscribe(params => {
        if (params['returnUrl']) {
          this.returnUrl = params['returnUrl'];
        } else {
          this.returnUrl = base64url.decode(params['encodedReturnUrl']);
        }
      });

    this.errors = {'hasErrors': 'false'};
    this.formValidation = false;
    this.loginInProcess = false;
    this.loader.useDimmer = false;

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
    if (this.loginInProcess) {
      return;
    }
    this.loginInProcess = true;
    this.formValidation = true;

    this.updateErrorMessages();
    if (this.errors['hasErrors'] === 'true') {
      this.loginInProcess = false;
      return;
    }

    const user = new UserLogin(
      this.loginForm.get('login').value,
      this.loginForm.get('password').value,
      this.returnUrl,
      this.token
    );

    this.loader.show('Anmeldung läuft...');
    this.service.login(user, () => {
      this.loginInProcess = false;
      this.loader.hide();
      },
      (message) => {
        this.loginInProcess = false;
        this.loader.hide();
        this.errorDialog.show('Fehler', message);
      }
    );
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
