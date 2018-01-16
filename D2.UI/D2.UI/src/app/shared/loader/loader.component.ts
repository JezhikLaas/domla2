import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'da-loader',
  template: `
    <div *ngIf="showLoader && useDimmer" class="ui active dimmer">
      <div class="ui big text loader">{{loaderText}}</div>
    </div>
    <div *ngIf="showLoader && !useDimmer" class="ui big active text loader">{{loaderText}}</div>
  `,
  styles: []
})

export class LoaderComponent implements OnInit {

  showLoader: boolean;
  loaderText: string;
  useDimmer: boolean;

  constructor() {
    this.showLoader = false;
    this.loaderText = 'Bitte warten...';
    this.useDimmer = true;
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
