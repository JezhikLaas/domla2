import {SubUnit} from './subunit';

export class BoundSubunit extends SubUnit {
  public Floor?; string;
  constructor (id: string, title: string, version: number, usage: string, number: number, floor: string) {
    super();
    this.Id = id;
    this.Title = title;
    this.Version = version;
    this.Number = number;
    this.Floor = floor;
  }
}
