import { Injectable } from '@angular/core';
import { Component, ViewContainerRef } from '@angular/core';
import { Overlay } from 'ngx-modialog';
import { Modal } from 'ngx-modialog/plugins/vex';

declare var $: any;
declare var vex: any;

@Component({
  selector: 'da-error-dialog',
  template: `<button (click)="onClick()">Alert</button>`,
  styles: []
})

@Injectable()
export class ErrorDialogComponent {

  constructor(public modal: Modal) {
  }

  show(title: string, message: string) {
    vex.defaultOptions.className = 'vex-theme-os';
    vex.dialog.alert({ unsafeMessage: `<div class="centered content" style="text-align:center">
                                         <h5>${title}</h5></div>
                                       ${message}`
    });
  }
}
