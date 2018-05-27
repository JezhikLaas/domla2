import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-loader',
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

  private defaultLoaderText = 'Bitte warten...';

  constructor() {
    this.showLoader = false;
    this.loaderText = '';
    this.useDimmer = true;
  }

  ngOnInit() {
  }

  show(text?: string) {
    if (text !== undefined) {
      this.loaderText = text;
    } else {
      this.loaderText = this.defaultLoaderText;
    }
    this.showLoader = true;
  }

  hide() {
    this.showLoader = false;
  }
}
