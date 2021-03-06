import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AddressService } from '../address.service';
import { PostalCodeInfo } from '../postal-code-info';
import { CountryInfo } from '../../../shared/country-info';
import { FormGroup, FormControl } from '@angular/forms';
import { List } from 'linqts';

@Component({
  selector: 'ui-address',
  templateUrl: './address-with-postalcode.component.html',
  styleUrls: ['./address-with-postalcode.component.css']
})
export class AddressWithPostalcodeComponent implements OnInit {
  PostalCodeInfo: PostalCodeInfo [];
  @Output() PostalCodeSelected = new EventEmitter<any>();
  @Input() AddressFormGroup: FormGroup;
  @Input() CountryFormGroup: FormGroup;
  @Input() Iso2Control: FormControl;
  @Input() CityControl: FormControl;
  @Input() StreetControl: FormControl;
  @Input() NumberControl: FormControl;
  @Input() PostalCodeControl: FormControl;
  Countries: CountryInfo[];
  CountryDefaultName: string;
  City = '';

  constructor(
    private as: AddressService) { }

  ngOnInit() {
    this.as.listPostalCodeInfo()
      .subscribe(res => {
        this.PostalCodeInfo = res;
      });

    this.as.getCountries().subscribe(res => {
      this.Countries = res;
      if (this.Iso2Control.value) {
        const CountryDefault = res.find (countries => countries.Iso2 === this.Iso2Control.value);
        if (CountryDefault) {
          this.CountryDefaultName = CountryDefault.Name;
          this.CountryFormGroup.patchValue( {Name: this.CountryDefaultName });
        }
      }
    });
  }

  onPostalCodeSelected(postcode: any, country: any) {
    this.PostalCodeSelected.emit(postcode);
    this.cityRefresh(postcode, country);
    this.postalCodeRefresh(postcode);
  }

  onCountrySelected( country: any, postcode: any) {
    this.countryRefresh(country);
    this.cityRefresh( postcode, country);
  }

  countryRefresh(iso2: any) {
    if (this.Countries) {
      const countryName = this.Countries.find(country => country.Iso2 === iso2 ).Name;
      this.CountryFormGroup.patchValue( {Name: countryName});
    }
  }

  postalCodeRefresh(postcode) {
    if (this.PostalCodeInfo) {
      const postalCodeinfo = this.PostalCodeInfo.find(pc => pc.PostalCode === postcode);
      if (postalCodeinfo) {
        this.PostalCodeControl.setValue(postalCodeinfo.PostalCode);
      }
    }
  }

  cityRefresh(postcode: any, country: any ) {
    if (this.PostalCodeInfo) {
      const postalCodeArray = new List<PostalCodeInfo>(this.PostalCodeInfo);
      const postalCodeInfo = postalCodeArray
        .FirstOrDefault(pc => pc.PostalCode === postcode && pc.Iso2 === country );
      if (postalCodeInfo) {
        this.CityControl.setValue( postalCodeInfo.City);
      }
    }
  }
}
