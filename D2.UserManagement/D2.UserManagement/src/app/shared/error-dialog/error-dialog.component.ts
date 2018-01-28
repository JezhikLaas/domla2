import { Injectable } from '@angular/core';
import { Component, ViewContainerRef } from '@angular/core';
import * as _ from 'underscore';

declare var $: any;
declare var vex: any;

@Component({
  selector: 'um-error-dialog',
  template: ``,
  styles: []
})

@Injectable()
export class ErrorDialogComponent {

  constructor() {
    vex.defaultOptions.className = 'vex-theme-os';
  }

  show(title: string, message: string) {
    const title_ = _.escape(title);
    const message_ = _.escape(message);
    vex.dialog.alert({ unsafeMessage: `<div class="centered content" style="text-align:center">
                                         <h5>${title_}</h5></div>
                                       ${message_}`
    });
  }
}
