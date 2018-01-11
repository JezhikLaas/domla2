import { Component, OnInit } from '@angular/core';

declare var $: any;

@Component({
  selector: 'da-error-dialog',
  templateUrl: './error-dialog.component.html',
  styles: []
})

export class ErrorDialogComponent implements OnInit {

  content: { };

  constructor() {
    this.content = { title: 'Hinweis', message: 'Leer' };
  }

  ngOnInit() {
  }

  show(title: string, message: string) {
    this.content['title'] = title;
    this.content['message'] = message;
    $('#ErrorDialog').modal('show');
  }
}
