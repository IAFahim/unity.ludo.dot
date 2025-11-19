#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

// This struct should match the ConfigStruct in NativeShare.cs
typedef struct ConfigStruct {
    const char* title;
    const char* message;
} ConfigStruct;

extern "C" {
    void showAlertMessage(ConfigStruct* conf) {
        NSString* title = conf ? [NSString stringWithUTF8String:conf->title] : @"";
        NSString* message = conf ? [NSString stringWithUTF8String:conf->message] : @"";

        dispatch_async(dispatch_get_main_queue(), ^{
            UIAlertController *alert = [UIAlertController alertControllerWithTitle:title
                                                                           message:message
                                                                    preferredStyle:UIAlertControllerStyleAlert];

            UIAlertAction *defaultAction = [UIAlertAction actionWithTitle:@"OK"
                                                                    style:UIAlertActionStyleDefault
                                                                  handler:^(UIAlertAction * action) {}];

            [alert addAction:defaultAction];

            UIViewController *rootViewController = [UIApplication sharedApplication].keyWindow.rootViewController;
            [rootViewController presentViewController:alert animated:YES completion:nil];
        });
    }
}