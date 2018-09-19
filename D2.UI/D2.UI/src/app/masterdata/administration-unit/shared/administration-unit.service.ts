import { throwError as observableThrowError,  Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AccountService } from '../../../shared/account.service';
import { AdministrationUnit } from './administration-unit';
import { IAdministrationUnitRaws} from './iadministration-unit-raws';
import { AdminUnitFactory } from './admin-unit-factory';
import { map, switchMap, catchError } from 'rxjs/operators';
import { SelectedAdministrationUnitsPropertyParameter } from './selected-administration-units-property-parameter';
import {forEach} from '@angular/router/src/utils/collection';
import { of } from 'rxjs';
import { Guid } from 'guid-typescript';
import { List } from 'linqts';
import { AdministrationUnitProperty } from './administration-unit-property';
import { getLocaleDateFormat } from '@angular/common';
import { Variant } from '../../../shared/variant';
import { SubUnit } from '../../subunit/subunit';

@Injectable()
export class AdministrationUnitService {
  administrationUnits:  any[];
  private topic = 'AdministrationUnit';
  private brokerUrl:  string;

  constructor(private http:  HttpClient,
              private accountService:  AccountService) {
    this.administrationUnits = [{
      UserKey: 'Ramker Weg 10',
      Title: 'Ramker Weg 10',
      Entrances: [
        {
          Title: '10a',
          Address:
            {
              Street: 'Ramker Weg',
              Number: '10a',
              Country:
                {
                  Iso2: 'DE',
                  Name: 'GERMANY'
                },
              PostalCode: '32051',
              City: 'Herford'
            },
          Version: 1,
          SubUnits: [
            {
              Floor: 'EG/OG',
              Title: '10a EG/OG',
              Number: 1,
              Version: 1,
              Id: '31a02109-4ce5-48bf-bcae-a94c006b6759',
              Edit: '0001-01-01T00: 00: 00'
            },
            {
              Floor: 'UG',
              Title: '10a UG',
              Number: 3,
              Version: 1,
              Id: 'b06264d1-d354-47fb-bec1-a94c006b675a',
              Edit: '0001-01-01T00: 00: 00'
            }],
          Id: 'a9534940-d406-46c1-8e32-a94c006b6757',
          Edit: '0001-01-01T00: 00: 00'
        },
        {
          Title: '10b',
          Address:
            {
              Street: 'Ramker Weg',
              Number: '10b',
              Country:
                {
                  Iso2: 'DE',
                  Name: 'GERMANY'
                },
              PostalCode: '32051',
              City: 'Herford'
            },
          Version: 1,
          SubUnits: [
            {
              Floor: 'EG/OG',
              Title: '10b EG/OG',
              Number: 2,
              Version: 1,
              Id: '5179c2d9-49d3-4316-a54f-a94c006b675a',
              Edit: '0001-01-01T00: 00: 00'
            },
            {
              Floor: 'UG',
              Title: '10b UG',
              Number: 4,
              Version: 1,
              Id: '39d9765c-0788-42ef-a434-a94c006b675a',
              Edit: '0001-01-01T00: 00: 00'
            }],
          Id: '55502094-2ce4-4b1a-9cf4-a94c006b675a',
          Edit: '0001-01-01T00: 00: 00'
        }],
      UnboundSubUnits: [
        {
          Type: 1,
          Title: 'Parkplatz 10a',
          Number: 5,
          Version: 1,
          Id: '587fd66d-7222-41bc-8211-a94c006b675b',
          Edit: '0001-01-01T00: 00: 00'
        },
        {
          Type: 1,
          Title: 'Parkplatz 10b',
          Number: 6,
          Version: 1,
          Id: '969c81b5-43a4-4a78-952f-a94c006b675b',
          Edit: '0001-01-01T00: 00: 00'
        }],
      SubUnits: [
        {
          Floor: 'EG/OG',
          Entrance:
            {
              Title: '10a',
              Address:
                {
                  Street: 'Ramker Weg',
                  Number: '10a',
                  Country: {Iso2: 'DE', Name: 'GERMANY'},
                  PostalCode: '32051',
                  City: 'Herford'
                },
              Version: 1,
              SubUnits: [
                {
                  Floor: 'UG',
                  Title: '10a UG',
                  Number: 3,
                  Version: 1,
                  Id: 'b06264d1-d354-47fb-bec1-a94c006b675a',
                  Edit: '0001-01-01T00: 00: 00'
                }],
              Id: 'a9534940-d406-46c1-8e32-a94c006b6757',
              Edit: '0001-01-01T00: 00: 00'
            },
          Title: '10a EG/OG',
          Number: 1,
          Version: 1,
          Id: '31a02109-4ce5-48bf-bcae-a94c006b6759',
          Edit: '0001-01-01T00: 00: 00'
        },
        {
          Floor: 'UG',
          Entrance:
            {
              Title: '10a',
              Address:
                {
                  Street: 'Ramker Weg',
                  Number: '10a',
                  Country: {Iso2: 'DE', Name: 'GERMANY'},
                  PostalCode: '32051',
                  City: 'Herford'
                },
              Version: 1,
              SubUnits: [
                {
                  Floor: 'EG/OG',
                  Title: '10a EG/OG',
                  Number: 1,
                  Version: 1,
                  Id: '31a02109-4ce5-48bf-bcae-a94c006b6759',
                  Edit: '0001-01-01T00: 00: 00'
                }],
              Id: 'a9534940-d406-46c1-8e32-a94c006b6757',
              Edit: '0001-01-01T00: 00: 00'
            },
          Title: '10a UG',
          Number: 3,
          Version: 1,
          Id: 'b06264d1-d354-47fb-bec1-a94c006b675a',
          Edit: '0001-01-01T00: 00: 00'
        },
        {
          Floor: 'EG/OG',
          Entrance:
            {
              Title: '10b',
              Address:
                {
                  Street: 'Ramker Weg',
                  Number: '10b',
                  Country: {Iso2: 'DE', Name: 'GERMANY'},
                  PostalCode: '32051',
                  City: 'Herford'
                },
              Version: 1,
              SubUnits: [
                {
                  Floor: 'UG',
                  Title: '10b UG',
                  Number: 4,
                  Version: 1,
                  Id: '39d9765c-0788-42ef-a434-a94c006b675a',
                  Edit: '0001-01-01T00: 00: 00'
                }],
              Id: '55502094-2ce4-4b1a-9cf4-a94c006b675a',
              Edit: '0001-01-01T00: 00: 00'
            },
          Title: '10b EG/OG',
          Number: 2,
          Version: 1,
          Id: '5179c2d9-49d3-4316-a54f-a94c006b675a',
          Edit: '0001-01-01T00: 00: 00'
        },
        {
          Floor: 'UG',
          Entrance:
            {
              Title: '10b',
              Address:
                {
                  Street: 'Ramker Weg',
                  Number: '10b',
                  Country: {Iso2: 'DE', Name: 'GERMANY'},
                  PostalCode: '32051',
                  City: 'Herford'
                },
              Version: 1,
              SubUnits: [
                {
                  Floor: 'EG/OG',
                  Title: '10b EG/OG',
                  Number: 2,
                  Version: 1,
                  Id: '5179c2d9-49d3-4316-a54f-a94c006b675a',
                  Edit: '0001-01-01T00: 00: 00'
                }],
              Id: '55502094-2ce4-4b1a-9cf4-a94c006b675a',
              Edit: '0001-01-01T00: 00: 00'
            },
          Title: '10b UG',
          Number: 4,
          Version: 1,
          Id: '39d9765c-0788-42ef-a434-a94c006b675a',
          Edit: '0001-01-01T00: 00: 00'
        },
        {
          Type: 1,
          Title: 'Parkplatz 10a',
          Number: 5,
          Version: 1,
          Id: '587fd66d-7222-41bc-8211-a94c006b675b',
          Edit: '0001-01-01T00: 00: 00'
        },
        {
          Type: 1,
          Title: 'Parkplatz 10b',
          Number: 6,
          Version: 1,
          Id: '969c81b5-43a4-4a78-952f-a94c006b675b',
          Edit: '0001-01-01T00: 00: 00'
        }],
      AdministrationUnitProperties: [
        {
          Title: 'Heizung',
          Description: null,
          Value: {Tag: 3, Raw: 'Erdwärme'},
          Version: 2,
          Id: '124969df-e174-4fff-9eb0-a94c006b675a',
          Edit: '0001-01-01T00: 00: 00'
        },
        {
          Title: 'Modernisierungsdatum',
          Description: null,
          Value: {Tag: 1, Raw: '2012-01-30T00:00:00'},
          Version: 2,
          Id: '124969df-e174-4fff-9eb0-a94c006b675a',
          Edit: '0001-01-01T00: 00: 00'
        },
        {
          Title: 'Fläche',
          Description: '1',
          Value:
            {
              Tag: 2,
              Raw: { _value: 56.0, _unit: 'qm', _decimalPlaces: 2 }
            },
          Version: 2,
          Id: '0032f567-bd63-4632-aee3-a95f00820bc0',
          Edit: '0001-01-01T00:00:00'
        }
      ],
      YearOfConstruction: {Year: 2011, Month: 11},
      Version: 1,
      Id: '28FB737F-B006-4E36-99A6-2F0CACDABE2B',
      Edit: '0001-01-01T00: 00: 00'
    },
    {
      UserKey: 'Ramker Weg 20',
      Title: 'Ramker Weg 20',
      Entrances: [
        {
          Title: '20a',
          Address:
            {
              Street: 'Ramker Weg',
              Number: '20a',
              Country:
                {
                  Iso2: 'DE',
                  Name: 'GERMANY'
                },
              PostalCode: '32051',
              City: 'Herford'
            },
          Version: 1,
          SubUnits: [
            {
              Floor: 'EG/OG',
              Title: '20a EG/OG',
              Number: 1,
              Version: 1,
              Id: '31a02109-4ce5-48bf-bcae-a94c006b6759',
              Edit: '0001-01-01T00: 00: 00'
            },
            {
              Floor: 'UG',
              Title: '20a UG',
              Number: 3,
              Version: 1,
              Id: 'b06264d1-d354-47fb-bec1-a94c006b675a',
              Edit: '0001-01-01T00: 00: 00'
            }],
          Id: 'a9534940-d406-46c1-8e32-a94c006b6757',
          Edit: '0001-01-01T00: 00: 00'
        },
        {
          Title: '20b',
          Address:
            {
              Street: 'Ramker Weg',
              Number: '20b',
              Country:
                {
                  Iso2: 'DE',
                  Name: 'GERMANY'
                },
              PostalCode: '32051',
              City: 'Herford'
            },
          Version: 1,
          SubUnits: [
            {
              Floor: 'EG/OG',
              Title: '20b EG/OG',
              Number: 2,
              Version: 1,
              Id: '5179c2d9-49d3-4316-a54f-a94c006b675a',
              Edit: '0001-01-01T00: 00: 00'
            },
            {
              Floor: 'UG',
              Title: '20b UG',
              Number: 4,
              Version: 1,
              Id: '39d9765c-0788-42ef-a434-a94c006b675a',
              Edit: '0001-01-01T00: 00: 00'
            }],
          Id: '55502094-2ce4-4b1a-9cf4-a94c006b675a',
          Edit: '0001-01-01T00: 00: 00'
        }],
      UnboundSubUnits: [
        {
          Type: 1,
          Title: 'Parkplatz 20a',
          Number: 5,
          Version: 1,
          Id: '587fd66d-7222-41bc-8211-a94c006b675b',
          Edit: '0001-01-01T00: 00: 00'
        },
        {
          Type: 1,
          Title: 'Parkplatz 20b',
          Number: 6,
          Version: 1,
          Id: '969c81b5-43a4-4a78-952f-a94c006b675b',
          Edit: '0001-01-01T00: 00: 00'
        }],
      SubUnits: [
        {
          Floor: 'EG/OG',
          Entrance:
            {
              Title: '20a',
              Address:
                {
                  Street: 'Ramker Weg',
                  Number: '20a',
                  Country: {Iso2: 'DE', Name: 'GERMANY'},
                  PostalCode: '32051',
                  City: 'Herford'
                },
              Version: 1,
              SubUnits: [
                {
                  Floor: 'UG',
                  Title: '20a UG',
                  Number: 3,
                  Version: 1,
                  Id: 'b06264d1-d354-47fb-bec1-a94c006b675a',
                  Edit: '0001-01-01T00: 00: 00'
                }],
              Id: 'a9534940-d406-46c1-8e32-a94c006b6757',
              Edit: '0001-01-01T00: 00: 00'
            },
          Title: '20a EG/OG',
          Number: 1,
          Version: 1,
          Id: '31a02109-4ce5-48bf-bcae-a94c006b6759',
          Edit: '0001-01-01T00: 00: 00'
        },
        {
          Floor: 'UG',
          Entrance:
            {
              Title: '20a',
              Address:
                {
                  Street: 'Ramker Weg',
                  Number: '20a',
                  Country: {Iso2: 'DE', Name: 'GERMANY'},
                  PostalCode: '32051',
                  City: 'Herford'
                },
              Version: 1,
              SubUnits: [
                {
                  Floor: 'EG/OG',
                  Title: '20a EG/OG',
                  Number: 1,
                  Version: 1,
                  Id: '31a02109-4ce5-48bf-bcae-a94c006b6759',
                  Edit: '0001-01-01T00: 00: 00'
                }],
              Id: 'a9534940-d406-46c1-8e32-a94c006b6757',
              Edit: '0001-01-01T00: 00: 00'
            },
          Title: '20a UG',
          Number: 3,
          Version: 1,
          Id: 'b06264d1-d354-47fb-bec1-a94c006b675a',
          Edit: '0001-01-01T00: 00: 00'
        },
        {
          Floor: 'EG/OG',
          Entrance:
            {
              Title: '20b',
              Address:
                {
                  Street: 'Ramker Weg',
                  Number: '20b',
                  Country: {Iso2: 'DE', Name: 'GERMANY'},
                  PostalCode: '32051',
                  City: 'Herford'
                },
              Version: 1,
              SubUnits: [
                {
                  Floor: 'UG',
                  Title: '20b UG',
                  Number: 4,
                  Version: 1,
                  Id: '39d9765c-0788-42ef-a434-a94c006b675a',
                  Edit: '0001-01-01T00: 00: 00'
                }],
              Id: '55502094-2ce4-4b1a-9cf4-a94c006b675a',
              Edit: '0001-01-01T00: 00: 00'
            },
          Title: '20b EG/OG',
          Number: 2,
          Version: 1,
          Id: '5179c2d9-49d3-4316-a54f-a94c006b675a',
          Edit: '0001-01-01T00: 00: 00'
        },
        {
          Floor: 'UG',
          Entrance:
            {
              Title: '20b',
              Address:
                {
                  Street: 'Ramker Weg',
                  Number: '20b',
                  Country: {Iso2: 'DE', Name: 'GERMANY'},
                  PostalCode: '32051',
                  City: 'Herford'
                },
              Version: 1,
              SubUnits: [
                {
                  Floor: 'EG/OG',
                  Title: '20b EG/OG',
                  Number: 2,
                  Version: 1,
                  Id: '5179c2d9-49d3-4316-a54f-a94c006b675a',
                  Edit: '0001-01-01T00: 00: 00'
                }],
              Id: '55502094-2ce4-4b1a-9cf4-a94c006b675a',
              Edit: '0001-01-01T00: 00: 00'
            },
          Title: '20b UG',
          Number: 4,
          Version: 1,
          Id: '39d9765c-0788-42ef-a434-a94c006b675a',
          Edit: '0001-01-01T00: 00: 00'
        },
        {
          Type: 1,
          Title: 'Parkplatz 20a',
          Number: 5,
          Version: 1,
          Id: '587fd66d-7222-41bc-8211-a94c006b675b',
          Edit: '0001-01-01T00: 00: 00'
        },
        {
          Type: 1,
          Title: 'Parkplatz 20b',
          Number: 6,
          Version: 1,
          Id: '969c81b5-43a4-4a78-952f-a94c006b675b',
          Edit: '0001-01-01T00: 00: 00'
        }],
      AdministrationUnitProperties: [
        {
          Title: 'Heizung',
          Description: null,
          Value: {Tag: 3, Raw: 'Erdwärme'},
          Version: 2,
          Id: '124969df-e174-4fff-9eb0-a94c006b675a',
          Edit: '0001-01-01T00: 00: 00'
        },
        {
          Title: 'Fläche',
          Description: '1',
          Value:
            {
              Tag: 2,
              Raw: { _value: 56.0, _unit: 'qm', _decimalPlaces: 2 }
            },
          Version: 2,
          Id: '0032f567-bd63-4632-aee3-a95f00820bc0',
          Edit: '0001-01-01T00:00:00'
        }],
      YearOfConstruction: {Year: 2011, Month: 11},
      Version: 1,
      Id: '567baf97-ab82-4681-847c-a94c006b674e',
      Edit: '0001-01-01T00: 00: 00'
      }
    ];
  }

  listAdministrationUnits():  Observable<Array<AdministrationUnit>> {
    return of(this.administrationUnits)
      .pipe(
        map(rawAdministrationUnits => rawAdministrationUnits
          .map(rawAdministrationUnit => AdminUnitFactory.fromObject(rawAdministrationUnit)))
      );
  }

  getListNoObservable(): AdministrationUnit[]  {
    return this.administrationUnits;
}
  getSingle(id:  string):  Observable<AdministrationUnit> {
    if (id !== '0') {
      // const adminUnitKopie = JSON.parse(JSON.stringify(this.administrationUnits));
      const list = new List <any>(this.administrationUnits) ;
      const adminUnit = list
        .Where(x  => x.Id === id )
        .FirstOrDefault();
      return of(JSON.parse(JSON.stringify(adminUnit)))
        .pipe(
          map(rawAdministrationUnit => AdminUnitFactory.fromObject(rawAdministrationUnit))
        );
    }
  }

  create(AdminUnit: AdministrationUnit): Observable<any> {
    const administrationUnit = AdminUnit;
    administrationUnit.Id = Guid.create().toString();
    this.pushSubUnits(administrationUnit);
    this.administrationUnits.push(administrationUnit);
    return of({newId: administrationUnit.Id});
  }

  edit (AdminUnit: AdministrationUnit): Observable<any> {
    const adminUnitIndex = this.administrationUnits.findIndex(item => item.Id === AdminUnit.Id);
    this.pushSubUnits(AdminUnit);
    this.administrationUnits[adminUnitIndex] = AdminUnit;
    return of([])
        .pipe(
          catchError(error => observableThrowError(error))
        );
  }

  pushSubUnits (AdminUnit: AdministrationUnit | any) {
    const boundSubUnits = [];
    for (const entrance of AdminUnit.Entrances) {
      for ( const subUnit of entrance.SubUnits) {
        const boundSubUnitWithEntrance = JSON.parse(JSON.stringify(subUnit));
        boundSubUnitWithEntrance.Entrance = entrance;
        boundSubUnits.push(boundSubUnitWithEntrance);
      }
    }
    AdminUnit.SubUnits = AdminUnit.UnboundSubUnits
      .concat(boundSubUnits);
  }

  addPropertiesSelectedAdministrationUnits(selectedAdministrationUnitsPropertyParameter: SelectedAdministrationUnitsPropertyParameter) {
    for (const adminUnitId of selectedAdministrationUnitsPropertyParameter.AdministrationUnitIds ) {
      const adminUnitProperty = new AdministrationUnitProperty(
        Guid.create().toString(),
        new Date(),
        1,
        selectedAdministrationUnitsPropertyParameter.AdministrationUnitsFeatureParameters.Title,
        this.buildPropertyValue(selectedAdministrationUnitsPropertyParameter.AdministrationUnitsFeatureParameters),
        selectedAdministrationUnitsPropertyParameter.AdministrationUnitsFeatureParameters.Description,
      );
      const list = new List <any>(this.administrationUnits);
      const adminUnit: AdministrationUnit = list
        .Where(x  => x.Id === adminUnitId )
        .FirstOrDefault();
      adminUnit.AdministrationUnitProperties.push(adminUnitProperty);
    }
    return of([])
      .pipe(
        catchError(error => observableThrowError(error))
      );
  }

 buildPropertyValue (feature: any): Variant {
    if (feature.Tag === 3 || feature.Tag === 1) {
      return new Variant (feature.Tag, null);
    } else if (feature.Tag === 2) {
      return new Variant (feature.Tag, { _value: null, _unit: feature.TypedValueUnit, _decimalPlaces: feature.TypedValueDecimalPlace });
    }
  }
}

