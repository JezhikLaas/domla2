import { Injectable } from '@angular/core';
import { Component, ViewContainerRef } from '@angular/core';

declare var $: any;
declare var vex: any;

@Component({
  selector: 'da-error-dialog',
  template: ``,
  styles: []
})

@Injectable()
export class ErrorDialogComponent {

  constructor() {
  }

  show(title: string, message: string) {
    vex.defaultOptions.className = 'vex-theme-os';
    vex.dialog.alert({ unsafeMessage: `<div class="centered content" style="text-align:center">
                                         <h5>${title}</h5></div>
                                       ${message}`
    });
  }
}
