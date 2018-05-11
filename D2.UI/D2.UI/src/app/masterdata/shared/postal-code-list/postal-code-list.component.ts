import {AfterContentInit, AfterViewInit, Component, DoCheck, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {AdministrationUnitService} from '../../adminunit/shared/administration-unit.service';
import {AddressService} from '../address.service';
import {IpostalCodeInfo} from '../ipostalcodeinfo';
import {CountryInfo} from '../../../shared/country-info';
import {FormGroup, FormControl} from '@angular/forms';
import {ControlValueAccessor} from '@angular/forms';

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
    private as: AddressService) { }

  ngOnInit() {
    this.as.listPostalCodeInfo()
      .subscribe(res => {
        this.PostalCodeInfo = res;
        if (this.PostalCode) {
          const postalCodeInfoDefault = res.find(plz => plz.PostalCode === this.PostalCode);
          if (postalCodeInfoDefault) {
            this.PostalCodeDefaultId = postalCodeInfoDefault.Id;
            this.City = postalCodeInfoDefault.City;
          }
        }
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

  onPostalCodeSelected(val: any) {
    this.PostalCodeSelected.emit(val);
    this.CityRefresh(val);
  }
  onCountrySelected(val: any, i: number) {
    this.CountryRefresh(val, i);
  }

  CountryRefresh(val: any, i: number) {
    if (this.Countries) {
      const countryName = this.Countries.find(country => country.Iso2 === val ).Name;
      this.CountryFormGroup.patchValue( {Name: countryName});
    }
  }

  CityRefresh(val) {
    this.City = this.PostalCodeInfo.find(plz => plz.Id === val).City;
  }
}
