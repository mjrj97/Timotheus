#!/bin/bash

# Notarization credentials
USERNAME="[REDACTED]"
PASSWORD="[REDACTED]"
BUNDLE_ID="dk.mjrj.Timotheus"

# Signing
APP_NAME="[PATH]/Timotheus" # Without extension
ENTITLEMENTS="[PATH]/Timotheus.entitlements"
APP_CERTIFICATE="Developer ID Application: [REDACTED] (XXXXXXXXXX)"
PKG_CERTIFICATE="Developer ID Installer: [REDACTED] (XXXXXXXXXX)"

find "$APP_NAME.app/Contents/MacOS/"|while read fname; do
    if [[ -f $fname ]]; then
        echo "[INFO] Signing $fname"
        codesign --force --timestamp --options=runtime --entitlements "$ENTITLEMENTS" --sign "$APP_CERTIFICATE" "$fname"
    fi
done

echo "[INFO] Signing app file"
codesign --force --timestamp --options=runtime --entitlements "$ENTITLEMENTS" --sign "$APP_CERTIFICATE" "$APP_NAME.app"

echo "[INFO] Zipping app file"
ditto -c -k --sequesterRsrc --keepParent "$APP_NAME.app" "$APP_NAME.zip"

echo "[INFO] Uploading App for Notarization"
xcrun altool --notarize-app --primary-bundle-id $BUNDLE_ID --username $USERNAME --password $PASSWORD --file "$APP_NAME.zip"

echo "[INFO] Deleting Zip file"
rm "$APP_NAME.zip"

echo "[INFO] Creating pkg file"
pkgbuild --component "$APP_NAME.app" --timestamp --install-location /Applications --sign "$PKG_CERTIFICATE" "$APP_NAME.pkg"

echo "[INFO] Uploading pkg for Notarization"
xcrun altool --notarize-app --primary-bundle-id $BUNDLE_ID --username $USERNAME --password $PASSWORD --file "$APP_NAME.pkg"