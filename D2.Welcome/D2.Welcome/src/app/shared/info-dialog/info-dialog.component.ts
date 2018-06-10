import { Component, Injectable} from '@angular/core';
import * as _ from 'underscore';

declare var $: any;
declare var vex: any;

@Component({
  selector: 'app-info-dialog',
  template: ``,
  styles: []
})

@Injectable()
export class InfoDialogComponent {

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
