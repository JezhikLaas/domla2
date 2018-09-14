import {SubUnit} from './subunit';

export class BoundSubunit extends SubUnit {
  public Floor?; string;
  constructor (id: string, title: string, version: number, usage: string, number: number, floor: string) {
    super(id, title, version, number);
    this.Floor = floor;
  }
}
