import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'da-loader',
  template: `
    <div *ngIf="showLoader" class="ui active dimmer">
      <div class="ui big text loader">{{loaderText}}</div>
    </div>
  `,
  styles: []
})

export class LoaderComponent implements OnInit {

  showLoader: boolean;
  loaderText: string;

  constructor() {
    this.showLoader = false;
    this.loaderText = 'Bitte warten...';
  }

  ngOnInit() {
  }

  show(text?: string) {
    if (text !== undefined) {
      this.loaderText = text;
    }
    this.showLoader = true;
  }

  hide() {
    this.showLoader = false;
  }
}
