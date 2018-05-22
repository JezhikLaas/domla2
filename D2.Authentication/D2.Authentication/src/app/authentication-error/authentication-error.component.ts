import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { map } from 'rxjs/operators';

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
      .pipe(map(params => params.get('error')))
      .subscribe(error => this.error['error'] = error);
    this.route
      .queryParamMap
      .pipe(map(params => params.get('description')))
      .subscribe(description => this.error['description'] = description);
  }
}
