import { Component, OnInit, ViewChild } from '@angular/core';
import { LoaderComponent } from './shared/loader/loader.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'app';
  @ViewChild(LoaderComponent) loader: LoaderComponent;

  ngOnInit(): void {
    this.loader.show();
    // this.loader.hide();
  }
}
