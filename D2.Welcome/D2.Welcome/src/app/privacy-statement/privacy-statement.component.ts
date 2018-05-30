import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-privacy-statement',
  templateUrl: './privacy-statement.component.html',
  styleUrls: ['./privacy-statement.component.css']
})
export class PrivacyStatementComponent implements OnInit {

  public statementAccepted: boolean;

  constructor() { }

  ngOnInit() {
  }

  acceptChange(value: boolean): void {
    this.statementAccepted = value;
  }

  state(): string {
    return this.statementAccepted ? '' : 'disabled';
  }
}
