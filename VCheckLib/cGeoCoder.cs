using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace VCheckLib
{
    public class cGeoCoder
    {

        double pLatitude = 0.0;
        double pLongitude = 0.0;

        public double Latitude
        {
            get 
            { 
                return pLatitude; 
            }
           
        }
        public double Longitude
        {
            get 
            { 
                return pLongitude; 
            }
           
        }

        public cGeoCoder(string StreetAddress, string Town, string Country)
        {
            try
            {
            string FullAddress = StreetAddress + ". " + Town + ". " + Country;
            //string sMapKey = "ABQIAAAASxtC6aF3X8Yduz285l4ARRTlVViOkHY_dKOjlNl-N3o0umkYTxSEF7047wST9i1Xzh4CBoq-vGTuKg";
            string sMapKey =   "ABQIAAAAYzWHj1VUYr87CfEy1ncZJxTAmcgP--YQ8_3z8Bko4NEx9oDvexQWRry8Gb8yUFURSyS6nOf4i1NSvw";
            

            Subgurim.Controles.GeoCode geocode = new Subgurim.Controles.GeoCode();
            Subgurim.Controles.GMap gmap1 = new Subgurim.Controles.GMap(); 
            
            geocode =  gmap1.getGeoCodeRequest(FullAddress, sMapKey);
            Subgurim.Controles.GLatLng gLatLng = new Subgurim.Controles.GLatLng(geocode.Placemark.coordinates.lat, geocode.Placemark.coordinates.lng);
            if (geocode != null)
            {
                pLatitude = geocode.Placemark.coordinates.lat;
                pLongitude = geocode.Placemark.coordinates.lng;
            }

            gmap1.setCenter(gLatLng, 16, Subgurim.Controles.GMapType.GTypes.Normal);
            Subgurim.Controles.GMarker oMarker = new Subgurim.Controles.GMarker(gLatLng);
            //Dim oMarker As New Subgurim.Controles.GMarker(gLatLng)
            //gmap1.addGMarker(oMarker);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
