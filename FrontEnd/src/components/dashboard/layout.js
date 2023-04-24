import React from 'react';

const Layout = ({ children, title, mode, onChangeSize, onRefresh, onClose }) => (
  <div className={'tile mode_' + mode}>
      <div className='header'>
        <div className='title'>{title}</div>
        <div className='actions'>
          <i onClick={() => onRefresh()} className="icon-refresh" title="refresh"></i>            
          {(mode < 3) && <i onClick={() => onChangeSize(1)} className="icon-enlarge" title="enlarge"></i> }
          {(mode > 1) && <i onClick={() => onChangeSize(-1)} className="icon-shrink" title="shrink"></i> }      
          <i onClick={() => onClose()} className="icon-close" title="close"></i>                  
        </div>   
      </div>   
      <div className="body">               
        {children}            
      </div>      
  </div>
);

export default Layout