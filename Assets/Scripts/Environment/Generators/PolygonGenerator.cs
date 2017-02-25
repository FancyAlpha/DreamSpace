using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class PolygonGenerator {
    public static MeshData polyfy(Vector3 offset , Vector3 stretch , int numOfVert , int spikeness) {

        //spikiness acts as density for now
        int Density = spikeness;

        MeshData polygon = new MeshData();
        polygon.vertices = new Vector3[(Density * (Density * 2))];
        polygon.triangles = new int[((Density - 1) * ((Density * 2) - 1)) * 6];



        int i = 0;
        int w = Density * 2;
        int h = Density;

        for ( int tita = 0; tita < Density; tita++ ) {//y
            double vtita = tita * ( (float)Math.PI / Density );

            for ( int nphi = -Density; nphi < Density; nphi++ ) {//x
                double vphi = nphi * ( (float)Math.PI / Density );

                //PointList(tita)(nphi + Density).X = Math.Sin(vtita) * Math.Cos(vphi);
                //PointList(tita)(nphi + Density).Y = Math.Sin(vtita) * Math.Sin(vphi);
                //PointList(tita)(nphi + Density).Z = Math.Cos(vtita);

                Debug.Log(i);

                polygon.vertices[i] = new Vector3( (float) Math.Sin(vtita) * (float) Math.Cos(vphi),
                                                   (float) Math.Sin(vtita) * (float) Math.Sin(vphi),
                                                   (float) Math.Cos(vtita));

                if ( nphi < Density - 1 && tita < Density - 1 ) {
                    polygon.addTriangle(i, i + w + 1, i + w);
                    polygon.addTriangle(i + w + 1 , i , i + 1);
                }

                i++;
            }
        }

        return polygon;
    }
}
