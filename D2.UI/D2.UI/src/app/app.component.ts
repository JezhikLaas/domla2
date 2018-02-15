import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { AccountService } from './shared/account.service';
import { ErrorDialogComponent } from './shared/error-dialog/error-dialog.component';
import { StorageService } from './shared/storage.service';
import { LoaderComponent } from './shared/loader/loader.component';
import 'rxjs/add/operator/map';
import { environment } from '../environments/environment';
import { Subscription } from 'rxjs/Subscription';
import { MenuDisplayService } from './shared/menu-display.service';

@Component({
  selector: 'ui-root',
  templateUrl: './app.component.html',
  styles: [`.navigation-container {
    position: absolute;
    top: 60px;
    bottom: 60px;
    left: 0;
    right: 0;
  }
  .navigation-sidenav {
    display: flex;
    align-items: center;
    justify-content: center;
    width: 200px;
    background: #006699;
  }
  .navigation-header {
    position: fixed;
    justify-content: center;
    top: 0;
    left: 0;
    right: 0;
  }
  .navigation-footer {
    position: fixed;
    justify-content: center;
    bottom: 0;
    left: 0;
    right: 0;
  }
  .navigation-content {
    justify-content: right;
    padding: 50px;
  }`]
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'Domla/2';
  @ViewChild(LoaderComponent) loader: LoaderComponent;
  MenuButtons: Array<string>;
  private subscription: Subscription;

  constructor(
    private accounts: AccountService,
    private errorDialog: ErrorDialogComponent,
    private cookieService: CookieService,
    private storage: StorageService,
    private menuDisplay: MenuDisplayService
  ) { }

  ngOnInit() {
    this.MenuButtons = [];
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

    this.subscription = this.menuDisplay.menuNeeded
      .subscribe((data: Array<string>) => {
        this.MenuButtons.length = 0;
        let item: any;
        for (item in data) {
          this.MenuButtons.push(data[item]);
        }
      });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  handleMenuNeeded(data: Array<string>) {
    this.MenuButtons = data;
  }

  logout() {
    this.accounts.logout(
      '',
      (message) => this.errorDialog.show('Fehler', message)
    );
  }
}
