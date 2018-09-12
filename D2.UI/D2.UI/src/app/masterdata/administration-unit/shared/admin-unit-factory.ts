import { AdministrationUnit } from './administration-unit';
import { IAdministrationUnitRaws } from './iadministration-unit-raws';
import {YearMonth} from '../../shared/year-month';
import {Variant} from '../../../shared/variant';
import {AdministrationUnitProperty} from './administration-unit-property';
import {AdministrationUnitSubunit} from './administration-unit-subunit';
import {Entrance} from '../../../shared/entrance';

export class AdminUnitFactory {
  static empty(): AdministrationUnit {
    return new AdministrationUnit('00000000-0000-0000-0000-000000000000', '', '', new Date(), 0, [
        {
          Id: '00000000-0000-0000-0000-000000000000',
          Title: '',
          Version: 0,
          Address: {
            Country:
              {Iso2: 'DE', Iso3: '', Name: ''},
            City: '',
            Street: '',
            Number: '',
            PostalCode: ''
          },
          Edit: '',
          AdministrationUnitId: '',
          SubUnits: []
        }],
      null,
      [],
      [],
      []
    );
  }

  static fromObject(rawAdministrationUnit: IAdministrationUnitRaws | any): AdministrationUnit {
    return new AdministrationUnit(
      rawAdministrationUnit.Id,
      rawAdministrationUnit.UserKey,
      rawAdministrationUnit.Title,
      typeof (rawAdministrationUnit.Edit) === 'string' ?
        new Date(rawAdministrationUnit.Edit) : rawAdministrationUnit.Edit,
      rawAdministrationUnit.Version,
      rawAdministrationUnit.Entrances,
        rawAdministrationUnit.YearOfConstruction,
      rawAdministrationUnit.AdministrationUnitProperties ? this.fromObjectBuildAdministrationUnitProperty(rawAdministrationUnit.AdministrationUnitProperties) :
                                rawAdministrationUnit.AdministrationUnitProperties,
      rawAdministrationUnit.UnboundSubUnits,
      this.fromObjectBuildSubUnits(rawAdministrationUnit.SubUnits),
    );
  }

  static fromObjectBuildSubUnits(subunits: any[]): AdministrationUnitSubunit[] {
    const subUnitsArr = new Array<AdministrationUnitSubunit>();
    for (let i = 0; i < subunits.length; i++ ) {
      const subUnit =  new AdministrationUnitSubunit(
        subunits[i].Id,
        subunits[i].Title,
        subunits[i].Version,
        subunits[i].Number,
        subunits[i].Floor ? subunits[i].Floor : '',
        subunits[i].Type ? subunits[i].Type : '',
        subunits[i].Entrance ? subunits[i].Entrance :
          new Entrance());
      subUnitsArr.push(subUnit);
    }
    return subUnitsArr;
  }

  static fromObjectBuildAdministrationUnitProperty(properties: any []): AdministrationUnitProperty[] {
    const propertyArray = new Array <AdministrationUnitProperty> ();
    for (let i = 0; i < properties.length; i++ ) {
      const property =  new AdministrationUnitProperty (
        properties[i].Id,
        properties[i].Edit,
        properties[i].Version,
        properties[i].Title,
        properties[i].Description,
        new Variant (properties[i].Value.Tag, properties[i].Value.Raw)
      );
      propertyArray.push(property);
    }
    return propertyArray;
  }

  static toObject(rawAdministrationUnit: IAdministrationUnitRaws | any): AdministrationUnit {
    return new AdministrationUnit(
      rawAdministrationUnit.Id,
      rawAdministrationUnit.UserKey,
      rawAdministrationUnit.Title,
      typeof (rawAdministrationUnit.Edit) === 'string' ?
        new Date(rawAdministrationUnit.Edit) : rawAdministrationUnit.Edit,
      rawAdministrationUnit.Version,
      rawAdministrationUnit.Entrances,
      rawAdministrationUnit.YearOfConstruction ?
        this.toObjectBuildYearOfConstruction(rawAdministrationUnit.YearOfConstruction) : rawAdministrationUnit.YearOfConstruction,
      rawAdministrationUnit.AdministrationUnitProperties ?
        this.toObjectBuildProperties(rawAdministrationUnit.AdministrationUnitProperties) :
        rawAdministrationUnit.AdministrationUnitProperties,
      rawAdministrationUnit.UnboundSubUnits
    );
  }

  static  toObjectBuildYearOfConstruction (yearOfConstruction: YearMonth | any): YearMonth {
    if (yearOfConstruction) {
      const date: Date = new Date(yearOfConstruction.toString());
      return new YearMonth(date.getFullYear(), date.getMonth() + 1);
    }
  }

  static toObjectBuildProperties (properties: AdministrationUnitProperty []): AdministrationUnitProperty [] {
    const propertyArray = new Array <AdministrationUnitProperty> ();
    for (let i = 0; i < properties.length; i++ ) {
       const property =  new AdministrationUnitProperty (
        properties[i].Id,
        properties[i].Edit,
        properties[i].Version,
        properties[i].Title,
        properties[i].Description,
        this.ToObjectBuildPropertyValue(properties[i])
      );
       propertyArray.push(property);
    }
    return propertyArray;
  }

  static ToObjectBuildPropertyValue (property: any): Variant {
    if (property.Value.Tag === 3 || property.Value.Tag === 1) {
      return new Variant (property.Value.Tag, property.Value.Raw);
    } else if (property.Value.Tag === 2) {
      return new Variant (property.Value.Tag, property.Value.RawNumber);
    }
  }
}
