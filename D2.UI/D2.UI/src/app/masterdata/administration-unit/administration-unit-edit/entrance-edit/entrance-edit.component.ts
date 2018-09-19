import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { List } from 'linqts';
import { PostalCodeInfo } from '../../../shared/postal-code-info';
import { CountryInfo } from '../../../../shared/country-info';
import { AddressService } from '../../../shared/address.service';
import { Entrance } from '../../../../shared/entrance';
import { AddressErrorMessages, EntranceErrorMessages } from '../administration-form-error-messages';
import { MenuItem } from '../../../../shared/menu-item';
import { ConfirmDialogComponent } from '../../../../shared/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'ui-entrance-edit',
  templateUrl: './entrance-edit.component.html',
  styles: []
})
export class EntranceEditComponent implements OnInit {
  PostalCodeInfo: PostalCodeInfo [];
  Entrance: FormGroup;
  Address: FormGroup;
  Country: FormGroup;
  Countries: CountryInfo[];
  CountryDefaultName: string;
  City = '';
  Errors: { [key: string]: string } = {};

  constructor(
    public dialogRef: MatDialogRef<EntranceEditComponent>,
    public fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public entrance: Entrance,
    public ad: AddressService
  ) { }

  ngOnInit() {
    this.initEntrance();
    this.ad.listPostalCodeInfo()
      .subscribe(res => {
        this.PostalCodeInfo = res;
      });

    this.ad.getCountries().subscribe(res => {
      this.Countries = res;
      if (this.Entrance.get(['Address', 'Country', 'Iso2']).value) {
        const CountryDefault = res.find (countries => countries.Iso2 === this.Entrance.get(['Address', 'Country', 'Iso2']).value);
        if (CountryDefault) {
          this.CountryDefaultName = CountryDefault.Name;
          this.Entrance.get(['Address', 'Country']).patchValue( {Name: this.CountryDefaultName });
        }
      }
    });
  }

  initEntrance() {
    this.buildAddress();
    this.Entrance = this.fb.group({
      Title: this.fb.control (this.entrance.Title, [Validators.required, Validators.maxLength(256)]),
      Address: this.Address,
      Id: this.fb.control(this.entrance.Id),
      Version: this.fb.control(this.entrance.Version),
      Edit: this.fb.control(this.entrance.Edit),
      SubUnits: this.fb.array(this.entrance.SubUnits)
    });
    this.Entrance.statusChanges.subscribe(() => this.updateErrorMessages());
  }

  buildAddress() {
    this.buildCountry();
    this.Address = this.fb.group({
      City: this.fb.control (this.entrance.Address.City, [Validators.required, Validators.maxLength(100)]),
      Street: this.fb.control (this.entrance.Address.Street, [Validators.required, Validators.maxLength(150)]),
      Number: this.fb.control (this.entrance.Address.Number, [Validators.required, Validators.maxLength(10)]),
      Country: this.Country,
      PostalCode: this.fb.control (this.entrance.Address.PostalCode, [Validators.required, Validators.maxLength(20)])
    });
  }

  buildCountry() {
    this.Country = this.fb.group({Iso2: this.entrance.Address.Country.Iso2, Name: this.entrance.Address.Country.Name}, { validator: Validators.required});
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onCountrySelected( country: any, postcode: any) {
    this.countryRefresh(country);
    this.cityRefresh( postcode, country);
  }

  countryRefresh(iso2: any) {
    if (this.Countries) {
      const countryName = this.Countries.find(country => country.Iso2 === iso2 ).Name;
      this.Entrance.get(['Address', 'Country']).patchValue( {Name: countryName});
    }
  }

  cityRefresh(postcode: any, country: any ) {
    if (this.PostalCodeInfo) {
      const postalCodeArray = new List<PostalCodeInfo>(this.PostalCodeInfo);
      const postalCodeInfo = postalCodeArray
        .FirstOrDefault(pc => pc.PostalCode === postcode && pc.Iso2 === country );
      if (postalCodeInfo) {
        this.Entrance.get(['Address', 'City']).setValue( postalCodeInfo.City);
      }
    }
  }

  onPostalCodeSelected(postcode: any, country: any) {
    this.cityRefresh(postcode, country);
    this.postalCodeRefresh(postcode);
  }

  postalCodeRefresh(postcode) {
    if (this.PostalCodeInfo) {
      const postalCodeinfo = this.PostalCodeInfo.find(pc => pc.PostalCode === postcode);
      if (postalCodeinfo) {
        this.Address.get(['PostalCode']).setValue(postalCodeinfo.PostalCode);
      }
    }
  }

  updateErrorMessages() {
    this.Errors = {};
    this.updateErrorMessagesAddress();
    this.updateErrorMessagesEntrance();
  }

  updateErrorMessagesEntrance() {
      for (const message of EntranceErrorMessages) {
        const control = this.Entrance.get([message.forControl]);
        if (control &&
          control.dirty &&
          control.invalid &&
          control.errors &&
          control.errors[message.forValidator] &&
          !this.Errors['Entrance' + message.forControl +  message.forValidator]) {
          this.Errors['Entrance' + message.forControl + message.forValidator] = message.text;
        }
      }
  }
  updateErrorMessagesAddress() {
      for (const message of AddressErrorMessages) {
        const control = this.Address.get([message.forControl]);
        if (control &&
          control.dirty &&
          control.invalid &&
          control.errors &&
          control.errors[message.forValidator] &&
          !this.Errors['Address' + message.forControl + message.forValidator]) {
          this.Errors['Address' + message.forControl + message.forValidator] = message.text;
        }
      }
  }
}
