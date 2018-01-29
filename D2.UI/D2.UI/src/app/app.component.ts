import { Component, OnInit, ViewChild } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { AccountService } from './shared/account.service';
import { ErrorDialogComponent } from './shared/error-dialog/error-dialog.component';
import { StorageService } from './shared/storage.service';
import { LoaderComponent } from './shared/loader/loader.component';
import 'rxjs/add/operator/map';
import { environment } from '../environments/environment';

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
    const token = this.cookieService.get('access_token');

    if (token) {
      this.loader.show();
      this.storage.set('access_token', token);

      this.accounts.fetchServices(
        () => this.loader.hide(),
        message => {
          this.loader.hide();
          this.errorDialog.show('Fehler', message);
        }
      );
    } else if (environment.production) {
        this.errorDialog.show('Fehler', 'Es konnte kein Zugriffstoken ermittelt werden!');
    }
  }

  logout() {
    this.accounts.logout(
      '',
      (message) => this.errorDialog.show('Fehler', message)
    );
  }
}
