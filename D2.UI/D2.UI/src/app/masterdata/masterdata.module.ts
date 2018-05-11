import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule} from '../shared/shared.module';
import { PostalCodeListComponent } from './shared/postal-code-list/postal-code-list.component';


@NgModule({
  imports: [
    CommonModule,
    SharedModule
  ],
  declarations: []
})
export class MasterdataModule { }
