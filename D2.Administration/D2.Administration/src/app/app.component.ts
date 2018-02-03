import { Component, OnInit } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { StorageService } from './shared/storage.service';
import { environment } from '../environments/environment';
import { ErrorDialogComponent } from './shared/error-dialog/error-dialog.component';

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
export class AppComponent implements OnInit {
  title = 'Domla/2';

  constructor(
    private cookieService: CookieService,
    private errorDialog: ErrorDialogComponent,
    private storage: StorageService
  ) { }

  ngOnInit() {
    const token = this.cookieService.get('access_token');

    if (token) {
      this.storage.set('access_token', token);
    } else if (environment.production) {
      this.errorDialog.show('Fehler', 'Es konnte kein Zugriffstoken ermittelt werden!');
    }
  }
}
