import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccountServiceService } from '../shared/account-service.service';
import { ErrorDialogComponent } from '../shared/error-dialog/error-dialog.component';
import base64url from 'base64url';

@Component({
  selector: 'da-logout-user',
  templateUrl: './logout-user.component.html',
  styles: []
})
export class LogoutUserComponent implements OnInit {

  logoutId: string;

  constructor(
    private route: ActivatedRoute,
    private service: AccountServiceService,
    private errorDialog: ErrorDialogComponent
  ) { }

  ngOnInit() {
    this.route
      .queryParamMap
      .map(params => params.get('logoutId'))
      .subscribe(value => {
        this.logoutId = base64url.decode(value);
      });
  }

  logout() {
    console.log('logout id: ' + this.logoutId)
    this.service.logout(
      this.logoutId,
      (message) => this.errorDialog.show('Fehler', message)
    );
  }
}
