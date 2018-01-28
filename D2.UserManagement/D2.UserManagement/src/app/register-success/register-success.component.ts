import { Component, OnInit } from '@angular/core';

declare var $: any;

@Component({
  selector: 'um-register-success',
  templateUrl: './register-success.component.html',
  styles: []
})
export class RegisterSuccessComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

  public show() {
    $('.ui.modal').modal('show');
  }
}
