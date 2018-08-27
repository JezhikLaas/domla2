import {ISubunit} from '../../subunit/isubunit';
import {IUnboundSubunit} from '../../subunit/iunboundsubunit';
import {IBoundSubunit} from '../../subunit/iboundsubunit';
import {Entrance} from '../../../shared/entrance';

export interface IAdministrationUnitSubunit extends IUnboundSubunit, IBoundSubunit {
  Entrance?: Entrance;
}
