import {AfterContentInit, AfterViewInit, Component, DoCheck, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {AdministrationUnitService} from '../../adminunit/shared/administration-unit.service';
import {AddressService} from '../address.service';
import {IpostalCodeInfo} from '../ipostalcodeinfo';
import {CountryInfo} from '../../../shared/country-info';
import {FormGroup, FormControl, FormBuilder} from '@angular/forms';
import {ControlValueAccessor} from '@angular/forms';
import { List } from 'linqts';

@Component({
  selector: 'ui-address',
  templateUrl: './postal-code-list.component.html',
  styles: []
})
export class PostalCodeListComponent implements OnInit {
  PostalCodeInfo: IpostalCodeInfo [];
  @Output() PostalCodeSelected = new EventEmitter<any>();
  @Input() PostalCode;
  @Input () AddressFormGroup: FormGroup;
  @Input() CountryFormGroup: FormGroup;
  @Input()Iso2Control: FormControl;
  @Input() CountryDefaultIso2;
  @Input() CityControl: FormControl;
  @Input() StreetControl: FormControl;
  @Input () PostalCodeControl: FormControl;
  PostalCodeDefaultId = '';
  isLoading = false;
  Countries: CountryInfo[];
  CountryDefaultName: string;
  City = '';

  constructor(
    private as: AddressService,
    private fb: FormBuilder) { }

  ngOnInit() {
    this.as.listPostalCodeInfo()
      .subscribe(res => {
        this.PostalCodeInfo = res;
      });

    this.as.getCountries().subscribe(res => {
      this.Countries = res;
      if (this.CountryDefaultIso2) {
        const CountryDefault = res.find (countries => countries.Iso2 === this.CountryDefaultIso2);
        if (CountryDefault) {
          this.CountryDefaultName = CountryDefault.Name;
          this.CountryFormGroup.patchValue( {Name: this.CountryDefaultName });
        }
      }
    });
  }

  onPostalCodeSelected(postcode: any, country: any) {
    this.PostalCodeSelected.emit(postcode);
    this.CityRefresh(postcode, country);
    this.PostalCodeRefresh(postcode);
  }
  onCountrySelected( country: any, postcode: any) {
    this.CountryRefresh(country);
    this.CityRefresh( postcode, country);
  }

  CountryRefresh(iso2: any) {
    if (this.Countries) {
      const countryName = this.Countries.find(country => country.Iso2 === iso2 ).Name;
      this.CountryFormGroup.patchValue( {Name: countryName});
    }
  }

  PostalCodeRefresh(postcode) {
    if (this.PostalCodeInfo) {
      const postalCodeinfo = this.PostalCodeInfo.find(pc => pc.PostalCode === postcode);
      if (postalCodeinfo) {
        this.PostalCodeControl.setValue(postalCodeinfo.PostalCode);
      }
    }

  }
  CityRefresh(postcode: any, country: any ) {
    if (this.PostalCodeInfo) {
      const postalCodeArray = new List<IpostalCodeInfo>(this.PostalCodeInfo);
      const postalCodeInfo = postalCodeArray
        .FirstOrDefault(pc => pc.PostalCode === postcode && pc.Iso2 === country );
      if (postalCodeInfo) {
        this.CityControl.setValue( postalCodeInfo.City);
      }
    }
  }
}
