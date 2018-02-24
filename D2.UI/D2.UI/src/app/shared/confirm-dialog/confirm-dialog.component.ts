import { Injectable } from '@angular/core';
import { Component, ViewContainerRef } from '@angular/core';
import * as _ from 'underscore';

declare var $: any;
declare var vex: any;

@Component({
  selector: 'ui-confirm-dialog',
  template: ``,
  styles: []
})

@Injectable()
export class ConfirmDialogComponent {
  constructor() {
    vex.defaultOptions.className = 'vex-theme-os';
  }

  show(title: string, message: string, result: (value: boolean)  => void) {
    const title_ = _.escape(title);
    const message_ = _.escape(message);
    vex.dialog.confirm({
      unsafeMessage: `<div class="centered content" style="text-align:center">
                        <h5>${title_}</h5>
                      </div>
                      ${message_}`,
      buttons: [
        $.extend({}, vex.dialog.buttons.YES, { text: 'Ja' }),
        $.extend({}, vex.dialog.buttons.NO, { text: 'Nein' })
      ],
      callback: function(value) {
        result(value);
      }
    });
  }
}
