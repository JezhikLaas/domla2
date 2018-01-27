import { Component, OnInit, ViewChild } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { AccountService } from './shared/account.service';
import { ErrorDialogComponent } from './shared/error-dialog/error-dialog.component';
import { StorageService } from './shared/storage.service';
import { LoaderComponent } from './shared/loader/loader.component';
import 'rxjs/add/operator/map';

@Component({
  selector: 'ui-root',
  templateUrl: './app.component.html',
  styles: []
})
export class AppComponent implements OnInit {
  title = 'ui';
  @ViewChild(LoaderComponent) loader: LoaderComponent;

  constructor(
    private accounts: AccountService,
    private errorDialog: ErrorDialogComponent,
    private cookieService: CookieService,
    private storage: StorageService
  ) { }

  ngOnInit() {
    this.loader.show();
    const token = this.cookieService.get('access_token');
    this.storage.set('access_token', token);

    this.accounts.fetchServices(
      () => this.loader.hide(),
      message => {
        this.loader.hide();
        this.errorDialog.show('Fehler', message);
      }
    );
  }

  logout() {
    this.accounts.logout(
      '',
      (message) => this.errorDialog.show('Fehler', message)
    );
  }
}
