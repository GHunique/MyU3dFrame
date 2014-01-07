using System;

public delegate void EventHandler(object sender,EventArgs e);

public interface xsyInterface  
{
	void AssetsLoaded(object sender,EventArgs e);
}
