import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'da-logged-out',
  template: `
    <h4 class="ui dividing header">Auf Wiedersehen!</h4>

    <div>
      <h5>Sie sind nun abgemeldet.</h5>
    </div>
    <br>
    <div>
      Dieses Fenster kann nun geschlossen werden.
    </div>
  `,
  styles: []
})
export class LoggedOutComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
