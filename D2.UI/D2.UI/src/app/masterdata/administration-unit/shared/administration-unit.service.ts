import {throwError as observableThrowError,  Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AccountService } from '../../../shared/account.service';
import { IAdministrationUnit } from './iadministration-unit';
import { AdministrationUnit } from './administration-unit';
import {IAdministrationUnitRaws} from './iadministration-unit-raws';
import { AdminUnitFactory } from './admin-unit-factory';
import { map, switchMap, catchError } from 'rxjs/operators';
import {IAdministrationUnitFeature} from '../../shared/IAdministrationUnitFeature';
import {ISelectedAdministrationUnitsPropertyParameter} from './iselected-administration-units-property-parameter';
import {forEach} from '@angular/router/src/utils/collection';

@Injectable()
export class AdministrationUnitService {
  administrationUnits:  IAdministrationUnitRaws[];
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
        }],
      YearOfConstruction: {Year: 2011, Month: 11},
      Version: 1,
      Id: '567baf97-ab82-4681-847c-a94c006b674e',
      Edit: '0001-01-01T00: 00: 00'
    }
    ];
  }

  listAdministrationUnits():  Observable<Array<AdministrationUnit>> {
    const observable = Observable.create(observer => {
      setTimeout(() => {
        const adminUnits = this.administrationUnits;
        observer.next(adminUnits);
        observer.complete()();
      }, 2000);
    })
    return observable;
  }

  getSingle(id:  string):  Observable<AdministrationUnit> {
    if (id !== '0') {
      if (this.brokerUrl) {
        return this.http
          .get<IAdministrationUnitRaws>(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=Load&id=${id}`)
          .pipe(
            map(rawAdministrationUnit => AdminUnitFactory.fromObject(rawAdministrationUnit))
          );
      } else {
        return this.accountService.fetchServices()
          .pipe(
            switchMap(data => {
              this.brokerUrl = data.Broker;
              return this.http
                .get<IAdministrationUnitRaws>(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=Load&id=${id}`)
                .pipe(
                  map(rawAdministrationUnit => AdminUnitFactory.fromObject(rawAdministrationUnit))
                );
            }),
            catchError(error => observableThrowError(error))
          );
      }
    }
  }
  create(AdminUnit:  IAdministrationUnit):  Observable<any> {
    if (this.brokerUrl) {
      return this.http
        .post(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=Create`, AdminUnit);
    } else {
      return this.accountService.fetchServices()
        .pipe(
          switchMap(data => {
            this.brokerUrl = data.Broker;
            return this.http
              .post(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=Create`, AdminUnit);
          }),
          catchError(error => observableThrowError(error))
        );
    }
  }

  edit (AdminUnit:  IAdministrationUnit):  Observable<any> {
    if (this.brokerUrl) {
      return this.http
        .put(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=Edit`, AdminUnit);
    } else {
      return this.accountService.fetchServices()
        .pipe(
          switchMap(data => {
            this.brokerUrl = data.Broker;
            return this.http
              .put(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=Edit`, AdminUnit);
          }),
          catchError(error => observableThrowError(error))
        );
    }
  }

  addPropertiesSelectedAdministrationUnits(selectedAdministrationUnitsPropertyParameter:  ISelectedAdministrationUnitsPropertyParameter) {
    if (this.brokerUrl) {
      return this.http
        .put(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=AddProperty`, selectedAdministrationUnitsPropertyParameter);
    } else {
      return this.accountService.fetchServices()
        .pipe(
          switchMap(data => {
            this.brokerUrl = data.Broker;
            return this.http
              .put(`${this.brokerUrl}/Dispatch?groups=md&topic=${this.topic}&call=AddProperty`, selectedAdministrationUnitsPropertyParameter);
          }),
          catchError(error => observableThrowError(error))
        );
    }
  }
}

