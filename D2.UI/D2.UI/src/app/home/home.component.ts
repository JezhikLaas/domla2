import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'ui-home',
  template: `
    <p>
      home works! Test Dynamic
    </p>
  `,
  styles: []
})
export class HomeComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
