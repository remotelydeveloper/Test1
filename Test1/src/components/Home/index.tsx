import * as React from 'react';
import { StateContext, signout } from '../../shared';
import { Employees } from './employees';
export const Home: React.FC<any> = (props: any) => {
  const { state, dispatch } = React.useContext(StateContext);

  const handleSignOut = async () => await signout(dispatch);

  return (
    <div className='row justify-content-between'>
      <div className='col col-12 mt-12'>
        <Employees />
      </div>
    </div>
  );
};
