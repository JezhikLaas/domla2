import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { LoaderComponent } from './shared/loader/loader.component';
import { ActivatedRoute } from '@angular/router';
import { StorageService } from './shared/storage.service';

declare var $: any;

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, AfterViewInit {
  title = 'Domla/2';
  @ViewChild(LoaderComponent) loader: LoaderComponent;

  constructor(
    private route: ActivatedRoute,
    private storage: StorageService
  ) {
    this.route.queryParams.subscribe(params => {
      storage.set('id', params['id']);
    });
  }

  ngOnInit(): void {
    this.loader.show();
    this.loader.hide();
  }

  ngAfterViewInit(): void {
    $('.ui.sticky')
      .sticky({
        context: '#appheader'
      });
  }
}
