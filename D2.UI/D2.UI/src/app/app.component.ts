import { Component, OnInit } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { AccountService } from './shared/account.service';
import { ErrorDialogComponent } from './shared/error-dialog/error-dialog.component';
import { StorageService } from './shared/storage.service';
import 'rxjs/add/operator/map';

@Component({
  selector: 'ui-root',
  templateUrl: './app.component.html',
  styles: []
})
export class AppComponent implements OnInit {
  title = 'ui';

  constructor(
    private accounts: AccountService,
    private errorDialog: ErrorDialogComponent,
    private cookieService: CookieService,
    private storage: StorageService
  ) { }

  ngOnInit() {
    const token = this.cookieService.get('access_token');
    this.storage.set('access_token', token);
  }

  logout() {
    this.accounts.logout(
      '',
      (message) => this.errorDialog.show('Fehler', message)
    );
  }
}
