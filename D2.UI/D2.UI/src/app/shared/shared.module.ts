import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ErrorDialogComponent } from './error-dialog/error-dialog.component';
import { LoaderComponent } from './loader/loader.component';
import { CdkTableModule } from '@angular/cdk/table';
import {FormsModule} from '@angular/forms';

import {
  MatAutocompleteModule,
  MatButtonModule,
  MatButtonToggleModule,
  MatCardModule,
  MatCheckboxModule,
  MatChipsModule,
  MatDatepickerModule,
  MatDialogModule,
  MatExpansionModule,
  MatGridListModule,
  MatIconModule,
  MatInputModule,
  MatListModule,
  MatMenuModule,
  MatNativeDateModule,
  MatPaginatorModule,
  MatProgressBarModule,
  MatProgressSpinnerModule,
  MatRadioModule,
  MatRippleModule,
  MatSelectModule,
  MatSidenavModule,
  MatSliderModule,
  MatSlideToggleModule,
  MatSnackBarModule,
  MatSortModule,
  MatTableModule,
  MatTabsModule,
  MatToolbarModule,
  MatTooltipModule,
  MatStepperModule,
} from '@angular/material';

import { ConfirmDialogComponent } from './confirm-dialog/confirm-dialog.component';
import { CommonModule } from '@angular/common';
import {PostalCodeListComponent} from '../masterdata/shared/postal-code-list/postal-code-list.component';
import {DateValueAccessorModule} from 'angular-date-value-accessor';



@NgModule({
  exports: [
    CdkTableModule,
    MatAutocompleteModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatCardModule,
    MatCheckboxModule,
    MatChipsModule,
    MatStepperModule,
    MatDatepickerModule,
    MatDialogModule,
    MatExpansionModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatNativeDateModule,
    MatPaginatorModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatRadioModule,
    MatRippleModule,
    MatSelectModule,
    MatSidenavModule,
    MatSliderModule,
    MatSlideToggleModule,
    MatSnackBarModule,
    MatSortModule,
    MatTableModule,
    MatTabsModule,
    MatToolbarModule,
    MatTooltipModule,
  ],
  declarations: []
})
export class UiMaterialModule {}

@NgModule({
  declarations: [
    ErrorDialogComponent,
    LoaderComponent,
    ConfirmDialogComponent,
    PostalCodeListComponent
  ],
  imports: [
    ReactiveFormsModule,
    HttpClientModule,
    UiMaterialModule,
    CommonModule,
    DateValueAccessorModule,
    FormsModule
  ],
  exports: [
    LoaderComponent,
    ErrorDialogComponent,
    ConfirmDialogComponent,
    UiMaterialModule,
    PostalCodeListComponent,
    DateValueAccessorModule
  ],

  schemas: [
    CUSTOM_ELEMENTS_SCHEMA
  ]
})
export class SharedModule { }
