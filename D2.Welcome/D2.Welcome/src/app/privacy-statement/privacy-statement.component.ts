import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-privacy-statement',
  templateUrl: './privacy-statement.component.html',
  styleUrls: ['./privacy-statement.component.css']
})
export class PrivacyStatementComponent implements OnInit {

  public statementAccepted: boolean;
  public stepOneActive: boolean;
  public stepTwoActive: boolean;
  public errors: { [key: string]: string};

  constructor() { }

  ngOnInit() {
    this.stepOneActive = true;
    this.errors = { 'passwordOne': null, 'passwordTwo': null };
  }

  acceptChange(value: boolean): void {
    this.statementAccepted = value;
  }

  state(): string {
    return this.statementAccepted ? '' : 'disabled';
  }

  commitAcceptance(): void {
    this.stepOneActive = false;
    this.stepTwoActive = true;
  }
}
