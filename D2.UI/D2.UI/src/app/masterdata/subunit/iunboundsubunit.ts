import {ISubunit} from './isubunit';

export interface IUnboundSubunit extends ISubunit {
  Type?: UnboundSubUnitType;
}

export enum UnboundSubUnitType {
  Parkplatz, Antenne
}

export namespace UnboundSubUnitType {
  export function values() {
    return Object.keys(UnboundSubUnitType).filter(
      (type) => isNaN(<any>type) && type !== 'values'
    );
  }
}
