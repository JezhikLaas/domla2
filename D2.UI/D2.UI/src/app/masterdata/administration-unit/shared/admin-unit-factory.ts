import { AdministrationUnit } from './administration-unit';
import { AdministrationUnitRaws } from './administration-unit-raws';
import {YearMonth} from '../../shared/year-month';
import {IAdministrationUnitProperty} from './iadministration-unit-property';
import {Variant} from '../../../shared/variant';
import {AdministrationUnitProperty} from './administration-unit-property';

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
          AdministrationUnitId: ''
        }],
      null,
      []
    );
  }

  static fromObject(rawAdministrationUnit: AdministrationUnitRaws | any): AdministrationUnit {
    return new AdministrationUnit(
      rawAdministrationUnit.Id,
      rawAdministrationUnit.UserKey,
      rawAdministrationUnit.Title,
      typeof (rawAdministrationUnit.Edit) === 'string' ?
        new Date(rawAdministrationUnit.Edit) : rawAdministrationUnit.Edit,
      rawAdministrationUnit.Version,
      rawAdministrationUnit.Entrances,
      typeof (rawAdministrationUnit.YearOfConstruction) === 'string' ?
        new YearMonth(rawAdministrationUnit.YearOfConstruction.Year, rawAdministrationUnit.YearOfConstruction.Month) :
        rawAdministrationUnit.YearOfConstruction,
      this.fromObjectBuildAdministrationUnitProperty(rawAdministrationUnit.AdministrationUnitProperties)
    );
  }

  static fromObjectBuildAdministrationUnitProperty(properties: any []): IAdministrationUnitProperty[] {
    const propertyArray = new Array <IAdministrationUnitProperty> ();
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

  static toObject(rawAdministrationUnit: AdministrationUnitRaws | any): AdministrationUnit {
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
        rawAdministrationUnit.AdministrationUnitProperties
    );
  }

  static  toObjectBuildYearOfConstruction (yearOfConstruction: YearMonth | any): YearMonth {
    if (yearOfConstruction) {
      const date: Date = new Date(yearOfConstruction.toString());
      const yearMonth: YearMonth = new YearMonth(date.getFullYear(), date.getMonth() + 1);
      return yearMonth;
    }
  }

  static toObjectBuildProperties (properties: IAdministrationUnitProperty []): IAdministrationUnitProperty [] {
    const propertyArray = new Array <IAdministrationUnitProperty> ();
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

  static convertPropertyValueRaw (properties: IAdministrationUnitProperty[], isUpdatingAdminUnit: boolean): IAdministrationUnitProperty[] {
    const auProperties = properties;
    if (isUpdatingAdminUnit) {
      for (let i = 0; i < properties.length; i++) {
        if (properties[i].Value.Tag === 1
          && typeof properties[i].Value.Raw === 'string') {
          auProperties[i].Value.Raw =
            new Date(properties[i].Value.Raw).toISOString();
        }
      }
    }
    return auProperties;
  }
}
