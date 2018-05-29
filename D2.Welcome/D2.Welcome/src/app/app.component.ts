import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { LoaderComponent } from './shared/loader/loader.component';

declare var $: any;

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, AfterViewInit {
  title = 'Domla/2';
  @ViewChild(LoaderComponent) loader: LoaderComponent;

  ngOnInit(): void {
    this.loader.show();
    this.loader.hide();
  }

  ngAfterViewInit(): void {
    $(document).ready(function() {
      $('.ui.sticky')
        .sticky({
          context: '#appheader'
        });
    });
  }
}
