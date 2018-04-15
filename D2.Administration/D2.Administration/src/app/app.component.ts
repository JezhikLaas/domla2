import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { StorageService } from './shared/storage.service';
import { environment } from '../environments/environment';
import { ErrorDialogComponent } from './shared/error-dialog/error-dialog.component';
import { NavigationEnd, Router } from '@angular/router';
import { MenuDisplayService } from './shared/menu-display.service';
import { Subscription } from 'rxjs/Subscription';
import { AdministrationService } from './shared/administration.service';
import { MenuItem } from './shared/menu-item';
import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/map';

@Component({
  selector: 'am-root',
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
    align-items: stretch;
    justify-content: center;
    width: 200px;
  }
  .navigation-header {
    background: #006699;
    position: fixed;
    justify-content: center;
    top: 0;
    left: 0;
    right: 0;
    color: whitesmoke;
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
  MenuButtons: Array<MenuItem>;
  private subscription: Subscription;

  constructor(
    private cookieService: CookieService,
    private errorDialog: ErrorDialogComponent,
    private storage: StorageService,
    private menuDisplay: MenuDisplayService,
    private service: AdministrationService,
    private changeDetection: ChangeDetectorRef,
    private router: Router
  ) {
    this.MenuButtons = [];
  }

  ngOnInit() {
    const access_token = this.cookieService.get('access_token');
    const refresh_token = this.cookieService.get('refresh_token');

    if (refresh_token) {
      this.storage.set('refreh_token', refresh_token);
    }

    if (access_token) {
      this.storage.set('access_token', access_token);
    } else if (environment.production) {
      this.errorDialog.show('Fehler', 'Es konnte kein Zugriffstoken ermittelt werden!');
    }

    this.subscription = this.menuDisplay.menuNeeded
      .subscribe((data: Array<MenuItem>) => {
        this.MenuButtons.length = 0;
        for (const item of data) {
          this.MenuButtons.push(item);
        }
        this.changeDetection.detectChanges();
      });

    this.router.events
      .filter((event) => event instanceof NavigationEnd)
      .subscribe((event) => {
        const navigationEnd = event as NavigationEnd;
        if (navigationEnd.url === '/') {
          this.MenuButtons.length = 0;
          this.changeDetection.detectChanges();
        }
      });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  handleMenuNeeded(data: Array<MenuItem>) {
    this.MenuButtons = data;
  }

  logout() {
    this.service.logout(
      (message) => this.errorDialog.show('Fehler', message)
    );
  }
}
