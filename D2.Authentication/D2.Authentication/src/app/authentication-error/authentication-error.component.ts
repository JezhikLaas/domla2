import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

@Component({
  selector: 'da-authentication-error',
  templateUrl: './authentication-error.component.html',
  styles: []
})

export class AuthenticationErrorComponent implements OnInit {
  error: { [key: string]: string };

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.error = { };
    this.route
      .queryParamMap
      .map(params => params.get('error'))
      .subscribe(error => this.error['error'] = error);
    this.route
      .queryParamMap
      .map(params => params.get('description'))
      .subscribe(description => this.error['description'] = description);
  }
}
