import { Component, OnInit } from '@angular/core';
import { AccountService } from './shared/account.service';
import { ErrorDialogComponent } from './shared/error-dialog/error-dialog.component';

@Component({
  selector: 'ui-root',
  templateUrl: './app.component.html',
  styles: []
})
export class AppComponent implements OnInit {
  title = 'ui';

  constructor(
    private accounts: AccountService,
    private errorDialog: ErrorDialogComponent
  ) { }

  ngOnInit() {}

  logout() {
    this.accounts.logout(
      '',
      (message) => this.errorDialog.show('Fehler', message)
    );
  }
}
