package com.example.zvi;

import android.app.Service;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.net.wifi.WifiManager;
import android.os.Environment;
import android.os.IBinder;
import android.support.annotation.Nullable;
import android.support.v4.app.NotificationCompat;
import android.support.v4.app.NotificationManagerCompat;
import android.widget.Toast;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileFilter;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.net.InetAddress;
import java.net.Socket;
import java.nio.ByteBuffer;
import java.nio.charset.Charset;
import java.util.ArrayList;
import java.util.List;

public class ImageServiceService extends Service {

    private final int NEW_FILE_COMMAND = 0;
    private BroadcastReceiver yourReceiver;
    private final IntentFilter theFilter = new IntentFilter();
    private boolean broadcasting;

    public void onDestroy() {
        Toast.makeText(this, "Service ending...", Toast.LENGTH_SHORT).show();
        // Unregisters the receiver so that your service will not listen for broadcasts
        this.unregisterReceiver(this.yourReceiver);
    }


    public int onStartCommand(Intent intent, int flag, int startId) {
        Toast.makeText(this, "Service starting...", Toast.LENGTH_SHORT).show();
        // Registers the receiver so that your service will listen for broadcasts
        this.registerReceiver(this.yourReceiver, theFilter);
        return START_STICKY;
    }

    public void onCreate() {
        super.onCreate();
        broadcasting = false;
        theFilter.addAction("android.net.wifi.supplicant.CONNECTION_CHANGE");
        theFilter.addAction("android.net.wifi.STATE_CHANGE");
        // final IntentFilter theFilter = new IntentFilter();
        this.yourReceiver = new BroadcastReceiver() {
            @Override
            public void onReceive(Context context, Intent intent) {

                WifiManager wifiManager = (WifiManager) context.getSystemService(Context.WIFI_SERVICE);
                NetworkInfo networkInfo = intent.getParcelableExtra(WifiManager.EXTRA_NETWORK_INFO);
                if (networkInfo != null) {
                    if (networkInfo.getType() == ConnectivityManager.TYPE_WIFI) {                    //get the different network states                    if (networkInfo.getState() == NetworkInfo.State.CONNECTED) {                        startTransfer();            // Starting the Transfer                    }                }            }        }    };    // Registers the receiver so that your service will listen for    // broadcasts    this.registerReceiver(this.yourReceiver, theFilter);}

                        //get the different network states
                        if (networkInfo.getState() == NetworkInfo.State.CONNECTED && !broadcasting) {
                            broadcasting = true;
                            final PendingResult result = goAsync();
                            Thread thread = new Thread() {
                                public void run() {
                                    startTransfer();            // Starting the Transfer                   }
                                    result.finish();
                                }
                            };
                            thread.start();
                            broadcasting = false;
                        }
                    }
                }
            }
        };
    }

    private void startTransfer() {

        //get files
        File dcim = Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_DCIM);
        if (dcim == null) {
            return;
        }

        ArrayList<File> pics = new ArrayList<>();
        getAllFiles(dcim, pics);


        if (pics.size() != 0) {
            //create notification bar
            NotificationManagerCompat notificationManager = NotificationManagerCompat.from(this);
            NotificationCompat.Builder builder = new NotificationCompat.Builder(this, "default");
            builder.setSmallIcon(R.drawable.ic_launcher_background);
            builder.setContentTitle("Picture Transfer").setContentText("Transfer in progress").setPriority(NotificationCompat.PRIORITY_LOW);
            int sum = pics.size();
            int count = 0;

            Socket socket = null;
            OutputStream output = null;
            FileInputStream fis = null;
            for (File pic : pics) {
                try {
                    //here you must put your computer's IP address.
                    InetAddress serverAddr = InetAddress.getByName("10.0.2.2");

                    //create a socket to make the connection with the server
                    socket = new Socket(serverAddr, 8000);
                    output = socket.getOutputStream();
                    // get byte array from pic
                    Bitmap bm;
                    fis = new FileInputStream(pic);
                    bm = BitmapFactory.decodeStream(fis);
                    byte[] imgbyte = getBytesFromBitmap(bm);
                    //send picture
                    /// send command
                    byte[] command = ByteBuffer.allocate(4).putInt(NEW_FILE_COMMAND).array();
                    output.write(command);
                    // send pic name and size
                    byte[] picLength = byteFlipper(ByteBuffer.allocate(4).putInt(pic.getName().length()).array());
                    output.write(picLength);
                    output.write(pic.getName().getBytes(Charset.forName("UTF-8")));
                    // send pic size
                    byte[] picSize = ByteBuffer.allocate(4).putInt(imgbyte.length).array();
                    output.write(byteFlipper(picSize));
                    // send pic
                    output.write(imgbyte);
                    output.flush();
                } catch (Exception e) {
                    System.out.println(e.getMessage());
                } finally {
                    try {
                        if (output != null)
                            output.close();
                        if (fis != null)
                            fis.close();
                        if (socket != null)
                            socket.close();
                    } catch (IOException e) {
                        e.printStackTrace();
                    }
                }

                builder.setContentText("Moving Pictures").setProgress(sum, count, false);
                notificationManager.notify(1, builder.build());
                count++;
            }
            // At the End
            builder.setContentText("Download complete").setProgress(0, 0, false);
            notificationManager.notify(1, builder.build());
        }
    }

    public byte[] byteFlipper(byte[] bytes) {
        int length = bytes.length;
        byte[] ans = new byte[length];
        for (int i = 0; i < length; i++) {
            ans[length - 1 - i] = bytes[i];
        }

        return ans;
    }

    public byte[] getBytesFromBitmap(Bitmap bitmap) {
        ByteArrayOutputStream stream = new ByteArrayOutputStream();
        bitmap.compress(Bitmap.CompressFormat.PNG, 70, stream);
        return stream.toByteArray();
    }


    public void getAllFiles(File folder, List<File> pics) {
        File[] files = folder.listFiles();

        if (files == null)
            return;

        for (File f : files) {
            if (f.isDirectory())
                getAllFiles(f, pics);
            else if (f.isFile())
                pics.add(f);
        }
    }

    @Nullable
    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }


}
